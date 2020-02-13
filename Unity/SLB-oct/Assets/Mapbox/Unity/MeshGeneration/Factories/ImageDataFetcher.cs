using Mapbox.Map;
using Mapbox.Platform;
using Mapbox.Unity.MeshGeneration.Data;
using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.Networking;

public class ImageDataFetcher : DataFetcher
{
	public Action<UnityTile, RasterTile> DataRecieved = (t, s) => { };
    public Action<UnityTile, RasterTile, Layers> LayerDataRecieved = (t, s, l) => { };
    public Action<UnityTile, byte[], Layers> LayerRawDataRecieved = (t, s, l) => { };

    public Action<UnityTile, RasterTile, TileErrorEventArgs> FetchingError = (t, r, s) => { };

    private string ElevationHash = "\"6c37276a93586c8cc06c64aded835a19\"";
    private string PorosityHash = "\"6530736b4619ea51bdab7ddd852bb07e\"";
    private string ThicknessHash = "\"ab4e840606782a3201b3d5df5e7a4258\"";
    private string TOCHash = "\"0f4c4db50425eaf8c621e0fc8e11ecbb\"";
    private string VShaleHash = "\"b90496b9027ad12fc484f9a3eca78d5b\"";

    private string enlargeURL = "https://em-dev-mincurvature-wip.maplarge.net/api/processdirect?request={\"action\":\"tile/getrawtiledata\",\"hash\":\"a755f40726075818a98d56b932df19f6\"}&uparams=x:${X};y:${Y};z:${Z};band:Vshale";
       

    private static GameObject CoroutineParent;

    //tile here should be totally optional and used only not to have keep a dictionary in terrain factory base
    public override void FetchData(DataFetcherParameters parameters, bool useMapBox = true)
	{
        if (CoroutineParent == null)
        {
            CoroutineParent = new GameObject();
            CoroutineParent.name = "CoroutineParent";
        }

		var imageDataParameters = parameters as ImageDataFetcherParameters;
		if(imageDataParameters == null)
		{
			return;
		}

        RasterTile rasterTile;

        if (imageDataParameters.mapid.StartsWith("mapbox://", StringComparison.Ordinal))
		{
			rasterTile = imageDataParameters.useRetina ? new RetinaRasterTile() : new RasterTile();
        }
		else
		{
			rasterTile = imageDataParameters.useRetina ? new ClassicRetinaRasterTile() : new ClassicRasterTile();
        }

		if (imageDataParameters.tile != null)
		{
			imageDataParameters.tile.AddTile(rasterTile);
		}

        TileInitializer(rasterTile, _fileSource, imageDataParameters.tile, imageDataParameters.mapid);


        bool useMaplarge = false; //!useMapBox;
       
        if (useMapBox)
        {
            RasterTile elevationTile = imageDataParameters.useRetina ? new ClassicRetinaRasterTile() : new ClassicRasterTile();
            RasterTile porosityTile = imageDataParameters.useRetina ? new ClassicRetinaRasterTile() : new ClassicRasterTile();
            RasterTile thicknessTile = imageDataParameters.useRetina ? new ClassicRetinaRasterTile() : new ClassicRasterTile();
            RasterTile TOCTile = imageDataParameters.useRetina ? new ClassicRetinaRasterTile() : new ClassicRasterTile();
            RasterTile vShaleTile = imageDataParameters.useRetina ? new ClassicRetinaRasterTile() : new ClassicRasterTile();

            TileInitializer(elevationTile, _fileSource, imageDataParameters.tile, "victer.0fd7kryp", Layers.Elevation, true);
            TileInitializer(porosityTile, _fileSource, imageDataParameters.tile, "victer.2w2lzfug", Layers.Porosity, true);
            TileInitializer(thicknessTile, _fileSource, imageDataParameters.tile, "victer.3totsqo7", Layers.Thickness, true);
            TileInitializer(TOCTile, _fileSource, imageDataParameters.tile, "victer.3df23l89", Layers.TOC, true);
            TileInitializer(vShaleTile, _fileSource, imageDataParameters.tile, "victer.cejjf2l8", Layers.VShale, true);
        }
        else if (useMaplarge)
        {

            CanonicalTileId tID = imageDataParameters.tile.CanonicalTileId;

            GameObject temp = new GameObject();
            temp.name = "Temp_"  + tID;
            temp.transform.SetParent(CoroutineParent.transform);
            MonoCoroutine mc = temp.AddComponent<MonoCoroutine>();


            string url = enlargeURL.Replace("${X}", tID.X.ToString())
                .Replace("${Y}", tID.Y.ToString())
                .Replace("${Z}", tID.Z.ToString());
            
            mc.GetGZipExternalMapData(url, InvokeCallback);

            void InvokeCallback(byte[] data)
            {
                LayerRawDataRecieved(imageDataParameters.tile, data, Layers.VShale);
            }
        }
        else
        {
            CanonicalTileId tID = imageDataParameters.tile.CanonicalTileId;

            foreach (Layers layer in (Layers[])Enum.GetValues(typeof(Layers)))
            {
                GameObject temp = new GameObject();
                temp.name = "Temp_" + layer + "_" + tID;
                temp.transform.SetParent(CoroutineParent.transform);
                MonoCoroutine mc = temp.AddComponent<MonoCoroutine>();

                string hash;

                switch (layer)
                {
                    case Layers.Elevation: hash = ElevationHash;  break;
                    case Layers.Porosity: hash = PorosityHash; break;
                    case Layers.Thickness: hash = ThicknessHash; break;
                    case Layers.TOC: hash = TOCHash; break;
                    case Layers.VShale: hash = VShaleHash; break;
                    default: return;
                }
                           
                string address = @"https://energy.maplarge.com/Api/ProcessDirect?request={""action"":""tile/getmultitile"",""hash"":" + hash + "}&uParams=x:" + tID.X + ";y:" + tID.Y + ";z:" + tID.Z + ";w:1;h:1;layeronly:true;debug:false";

                mc.GetExternalMapData(address, InvokeCallback);

                void InvokeCallback(byte[] data)
                {
                    LayerRawDataRecieved(imageDataParameters.tile, data, layer);
                }
            }
        }

        void TileInitializer(RasterTile tile, IFileSource fileSource, UnityTile uTile, string mapId, Layers layer = Layers.Elevation, bool useLayer = false)
        {
            tile.Initialize(fileSource, uTile.CanonicalTileId, mapId, () =>
            {
                if (uTile.CanonicalTileId != tile.Id)
                {
                    //this means tile object is recycled and reused. Returned data doesn't belong to this tile but probably the previous one. So we're trashing it.
                    return;
                }

                if (tile.HasError)
                {
                    FetchingError(uTile, tile, new TileErrorEventArgs(uTile.CanonicalTileId, tile.GetType(), uTile, tile.Exceptions));
                }
                else
                {
                    if (useLayer)
                    {
                        LayerDataRecieved(uTile, tile, layer);
                    }
                    else
                    {
                        DataRecieved(uTile, tile);
                    }                    
                }
            });
        }
    }


    public class MonoCoroutine : MonoBehaviour
    {
        private string URL;
        private Action<byte[]> Callback;

        public void GetExternalMapData(string url, Action<byte[]> callback)
        {
            URL = url;
            Callback = callback;

            StartCoroutine(GetMapData());
        }

        private IEnumerator GetMapData()
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(URL);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Destroy(gameObject);
            }
            else
            {
                Callback(www.downloadHandler.data);
                Destroy(gameObject);
            }
        }

        internal void GetGZipExternalMapData(string url, Action<byte[]> invokeCallback)
        {
            URL = url;

            Callback = (byte[] array) =>
            {
                var res = Decompress(array);
                invokeCallback(res);
            };

            StartCoroutine(GetMapData());


            byte[] Decompress(byte[] arrayGZipped)
            {
                byte[] decodedBytes;
                using (var gzipStream = new GZipStream(new MemoryStream(arrayGZipped), CompressionMode.Decompress))
                {
                    byte[] buffer = new byte[16 * 1024];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = gzipStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        decodedBytes = ms.ToArray();
                    }
                }
                return decodedBytes;
            };
        }
    }
}
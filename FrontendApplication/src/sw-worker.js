/* eslint-disable max-len */
/* eslint-env browser, serviceworker, es6 */

'use strict';

/**
 * Welcome to your Workbox-powered service worker!
 *
 * You'll need to register this file in your web app and you should
 * disable HTTP caching for this file too.
 * See https://goo.gl/nhQhGp
 *
 * The rest of the code is auto-generated. Please don't update this file
 * directly; instead, make changes to your Workbox build configuration
 * and re-run your build process.
 * See https://goo.gl/2aRDsh
 */

importScripts("https://storage.googleapis.com/workbox-cdn/releases/4.1.1/workbox-sw.js");
workbox.setConfig({
  debug: true,
  // modulePathPrefix: 'workbox-4.1.1/'
});


workbox.core.skipWaiting();
workbox.core.clientsClaim();

workbox.precaching.addPlugins([
  new workbox.broadcastUpdate.Plugin('precache-updates')
]);
workbox.precaching.precacheAndRoute([], {
  cleanUrls: false,
  directoryIndex: 'index.html',
  ignoreUrlParametersMatching: [/^utm_/],
});

// Cache version, update to trigger a new service worker activation
var staticCacheName = "\cache-v1.0";

// Cache js and css
const networkFirstStrategy = new workbox.strategies.NetworkFirst({
  cacheName: staticCacheName
});
workbox.routing.registerRoute(
  new RegExp('.*\.(?:js|css)'), networkFirstStrategy
);

const cacheFirstStrategy1 = new workbox.strategies.CacheFirst({
  cacheName: staticCacheName,
  plugins: [
    new workbox.expiration.Plugin({
      maxEntries: 60,
      maxAgeSeconds: 30 * 24 * 60 * 60, // 30 Days
    }),
  ],
});
// Cache images
workbox.routing.registerRoute(
  new RegExp('\.(?:png|gif|jpg|jpeg|svg|ico)$'), cacheFirstStrategy1
);

const cacheFirstStrategy2 = new workbox.strategies.CacheFirst({
  cacheName: 'google-fonts-webfonts',
  plugins: [
    new workbox.cacheableResponse.Plugin({
      statuses: [0, 200],
    }),
    new workbox.expiration.Plugin({
      maxAgeSeconds: 60 * 60 * 24 * 365,
      maxEntries: 30,
    }),
  ],
});
// Cache font files with a cache-first strategy for 1 year.
workbox.routing.registerRoute(
  /^https:\/\/fonts\.gstatic\.com/, cacheFirstStrategy2
);

const bgSyncPlugin = new workbox.backgroundSync.Plugin('myQueueName', {
  maxRetentionTime: 24 * 60 // Retry for max of 24 Hours
});

const myCheckFailPlugin = {
  fetchDidFail: ({originalRequest, request, error, event}) => {
      console.log('[fetchDidFail Worker]: ', originalRequest, error);
  }
};

const networkOnlyStrategy = new workbox.strategies.NetworkOnly({
  plugins: [bgSyncPlugin, myCheckFailPlugin]
});
workbox.routing.registerRoute(
  'https://hooks.slack.com/services/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', networkOnlyStrategy,
  'POST'
);

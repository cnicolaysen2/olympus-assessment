module.exports = {
  globDirectory: 'dist/',
  globPatterns: [
    '**/*.{html,css,json,svg,png,jpg,js,ico}',
    'assets/**/*'
  ],
  dontCacheBustURLsMatching: new RegExp('.+\.[a-f0-9]{20}\..+'),
  maximumFileSizeToCacheInBytes: 10 * 1024 * 1024,
  swDest: './dist/sw-worker.js',
  swSrc: './src/sw-worker.js'
};

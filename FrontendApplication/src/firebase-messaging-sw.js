
// Push notifications Event
self.addEventListener('push', function(event) {
  event.stopPropagation();
  const msg = event.data.json();

  console.log(`>>>>>>>>> [Service Worker] Push had this data: "${msg}"`);

  const title = msg.data.title;
  const options = {
    body: msg.data.body,
    icon: msg.data.icon,
    badge: msg.data.badge,
    click_action : msg.data.click_action,
    image: msg.data.image,
    timestamp: msg.data.timestamp,
    color: msg.data.color,
    actions: [
      {
        action: 'coffee-action',
        title: 'Coffee',
        icon: 'assets/icons/Oil.png',
      },
      {
        action: 'doughnut-action',
        title: 'Doughnut',
        icon: 'assets/icons/Shape.png',
      }
    ]
  };

  event.waitUntil(self.registration.showNotification(title, options));
});

self.addEventListener('notificationclick', function(event) {
  console.log('[Service Worker] Notification click Received.');

  event.notification.close();

  if (event.action === 'coffee-action') {
    event.waitUntil(
      clients.openWindow('https://www.starbucks.com')
    );

  } else if (event.action === 'doughnut-action') {
    event.waitUntil(
      clients.openWindow('https://krispykreme.com')
    );
  } else {
    event.waitUntil(
      clients.openWindow('https://homer-doughnuts.appspot.com')
    );
  }

});

const functions = require('firebase-functions');

// // Create and Deploy Your First Cloud Functions
// // https://firebase.google.com/docs/functions/write-firebase-functions
//
// exports.helloWorld = functions.https.onRequest((request, response) => {
//  response.send("Hello from Firebase!");
// });

const admin = require('firebase-admin');
admin.initializeApp();

const db = admin.firestore();

exports.averageFilter = functions.firestore
    .document('OlympusStore/randomwalk')
    .onUpdate((change, context) => {
        // Get an object representing the document
        const newValue = change.after.data();
        const oldValue = change.before.data();

        const newVal = (newValue.tip + oldValue.tip) / 2.0;

        db.doc('OlympusStore/filtered').set({
            out: newVal
          }, {merge: true});
    });

const express = require('express');
const router = express.Router();
const mysql2 = require('mysql2');

/* GET home page. */
router.get('/', function (req, res, next) {
    // return ALL rows from the table, but add a position column based on overall score
    const sql = `SELECT *, @rownum := @rownum + 1 AS position FROM (SELECT @rownum := 0) r, (SELECT * FROM scores ORDER BY overall DESC) s;`;
    connection.query(sql, function (err, results, fields) {
        if (err) {
            console.error(err);
        }

        // pass the results to the view
        res.render('index', { title: 'High Scores', jamData: results });
    });
});

router.get('/faq', function (req, res, next) {
    res.render('faq', { title: 'FAQ' });
});

router.post('/hook', function (req, res, next) {

    let data = {
        username: req.body.name,
        jam_json: JSON.stringify(req.body.jam),
        taste: req.body.jam.score.taste,
        combo: req.body.jam.score.combination,
        reaction: ['Heavenly', 'Good', 'Neutral', 'Bad', 'Spicy', 'Sour', 'Horrible'][req.body.jam.score.reaction],
        overall: req.body.jam.score.overall
    };

    connection.query('INSERT INTO `scores` SET ?', data, function (e, result) {
        if (e) {
            console.error(e);
            return res.sendStatus(500);
        }

        // All rows in the table have an overall score. Return the table position of the new score.
        connection.query('SELECT COUNT(*) AS position FROM scores WHERE overall >= ?', [data.overall], function (e, result) {
            if (e) {
                console.error(e);
                return res.sendStatus(500);
            }

            res.send("#" + result[0].position);
        });
    });
});

module.exports = router;

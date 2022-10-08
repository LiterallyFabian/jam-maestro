let express = require('express');
let path = require('path');
let sassMiddleware = require('node-sass-middleware');
const http = require('http');
const mysql2 = require("mysql2");
require('dotenv').config();

let indexRouter = require('./routes/index');

let app = express();

// Connect to database
connection = mysql2.createConnection({
  host: process.env.mysql_host || 'localhost',
  user: process.env.mysql_user,
  password: process.env.mysql_password,
  database: process.env.mysql_database
});

connection.connect(function (e) {
  if (e) {
    console.error(e);
  }

  console.log('Connected to the MySQL server ' + process.env.mysql_database);
});

// view engine setup
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'ejs');

app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(sassMiddleware({
  src: path.join(__dirname, 'public'),
  dest: path.join(__dirname, 'public'),
  indentedSyntax: false, // true = .sass and false = .scss
  sourceMap: true
}));
app.use(express.static(path.join(__dirname, 'public')));

app.use('/', indexRouter);

module.exports = app;

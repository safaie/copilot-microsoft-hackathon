//write npm command line to install mocha
//npm install --global mocha

//command to run this test file
//mocha test.js

const assert = require('assert');
const http = require('http');
const server = require('./nodeserver');

describe('Node Server', () => {
    it('should return "key not passed" if key is not passed', (done) => {
        http
        .get('http://localhost:3000/get' , (res) => {
            let data = '';
            res.on('data', (chunk) => {
                data += chunk;
            });
            res.on('end', () => {
                assert.equal(data, 'key not passed');
                done();
            });
        });
    });

    //add test to check get when key is equal to world
    it('should return "hello world" if key is equal to world', (done) => {
        http
        .get('http://localhost:3000/get?key=world', (res) => {
            let data = '';
            res.on('data', (chunk) => {
                data += chunk;
            });
            res.on('end', () => {
                assert.equal(data, 'hello world');
                done();
            });
        });
    });

    //add test to check validatephoneNumber
    it('should return true for a valid phone number', (done) => {
        http
        .get('http://localhost:3000/validatephoneNumber?number=1234567890', (res) => {
            let data = '';
            res.on('data', (chunk) => {
                data += chunk;
            });
            res.on('end', () => {
                assert.equal(data, 'true');
                done();
            });
        });
    });

    //write test to validate validateSpanishDNI
    it('should return true for a valid Spanish DNI', (done) => {
        http
        .get('http://localhost:3000/validateSpanishDNI?dni=12345678Z', (res) => {
            let data = '';
            res.on('data', (chunk) => {
                data += chunk;
            });
            res.on('end', () => {
                assert.equal(data, 'true');
                done();
            });
        });
    });

    //write test for returnColorCode red should return code #FF0000
    it('should return #FF0000 for color red', (done) => {
        http
        .get('http://localhost:3000/returnColorCode?color=red', (res) => {
            let data = '';
            res.on('data', (chunk) => {
                data += chunk;
            });
            res.on('end', () => {
                assert.equal(data, '#FF0000');
                done();
            });
        });
    });

    //write test for daysBetweenDates
    it('should return the correct number of days between two dates', (done) => {
        http
        .get('http://localhost:3000/daysBetweenDates?date1=2023-01-01&date2=2023-01-10', (res) => {
            let data = '';
            res.on('data', (chunk) => {
                data += chunk;
            });
            res.on('end', () => {
                assert.equal(data, '9');
                done();
            });
        });
    });

    //write test for returnRandomNumber
    it('should return a random number between 1 and 10', (done) => {
        http
        .get('http://localhost:3000/returnRandomNumber', (res) => {
            let data = '';
            res.on('data', (chunk) => {
                data += chunk;
            });
            res.on('end', () => {
                const number = parseInt(data, 10);
                assert(number >= 1 && number <= 10);
                done();
            });
        });
    });

    //write test for returnRandomString
    it('should return a random string of length 5', (done) => {
        http
        .get('http://localhost:3000/returnRandomString', (res) => {
            let data = '';
            res.on('data', (chunk) => {
                data += chunk;
            });
            res.on('end', () => {
                assert.equal(data.length, 5);
                done();
            });
        });
    });
});


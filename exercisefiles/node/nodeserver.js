const http = require('http');
const url = require('url');

const server = http.createServer((req, res) => {
    const parsedUrl = url.parse(req.url, true);
    const pathname = parsedUrl.pathname;
    const query = parsedUrl.query;

    if (pathname === '/get') {
        if (query.key === 'world') {
            res.end('hello world');
        } else {
            res.end('key not passed');
        }
    } else if (pathname === '/validatephoneNumber') {
        const isValid = /^\d{10}$/.test(query.number);
        res.end(isValid.toString());
    } else if (pathname === '/validateSpanishDNI') {
        const isValid = /^[0-9]{8}[A-Z]$/.test(query.dni);
        res.end(isValid.toString());
    } else if (pathname === '/returnColorCode') {
        const colorCodes = {
            red: '#FF0000',
            // ...other colors
        };
        res.end(colorCodes[query.color] || 'color not found');
    } else if (pathname === '/daysBetweenDates') {
        const date1 = new Date(query.date1);
        const date2 = new Date(query.date2);
        const diffTime = Math.abs(date2 - date1);
        const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
        res.end(diffDays.toString());
    } else if (pathname === '/returnRandomNumber') {
        const randomNumber = Math.floor(Math.random() * 10) + 1;
        res.end(randomNumber.toString());
    } else if (pathname === '/returnRandomString') {
        const randomString = Math.random().toString(36).substring(2, 7);
        res.end(randomString);
    } else {
        res.statusCode = 404;
        res.end('method not supported');
    }
});

server.listen(3000, () => {
    console.log('Server listening on port 3000');
});

module.exports = server;


const Mocha = require('mocha');
const mocha = new Mocha();

mocha.addFile('test/helloWorld.test.js');

mocha.run(function(failures) {
    process.exitCode = failures ? 1 : 0;
});
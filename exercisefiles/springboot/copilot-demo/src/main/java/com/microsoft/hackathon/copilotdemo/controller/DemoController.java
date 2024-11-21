package com.microsoft.hackathon.copilotdemo.controller;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.time.temporal.ChronoUnit;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

/* 
* Create a GET operation to return the value of a key passed as query parameter. 
* If the key is not passed, return "key not passed".
* If the key is passed, return "hello <key>".
* 
*/

@RestController
public class DemoController {
    private static final Logger logger = LoggerFactory.getLogger(DemoController.class);

    @GetMapping("/hello")
    public String hello(@RequestParam(required = false) String key) {
        if (key == null) {
            return "key not passed";
        }
        return "hello " + key;
    }

    @GetMapping("/diffdates")
    public String diffDates(@RequestParam String date1, @RequestParam String date2) {
        DateTimeFormatter formatter = DateTimeFormatter.ofPattern("dd-MM-yyyy");
        LocalDate startDate = LocalDate.parse(date1, formatter);
        LocalDate endDate = LocalDate.parse(date2, formatter);
        long daysBetween = ChronoUnit.DAYS.between(startDate, endDate);
        return "Difference in days: " + daysBetween;
    }

    @GetMapping("/validatePhoneNumber")
    public boolean validatePhoneNumber(@RequestParam String phoneNumber) {
        boolean isValid = phoneNumber.matches("\\+\\d{11,15}");
        logger.info("Validating phone number: {} - Result: {}", phoneNumber, isValid);
        return isValid;
    }

    @GetMapping("/validateDNI")
    public boolean validateDNI(@RequestParam String dni) {
        boolean isValid = dni.matches("\\d{8}[A-Za-z]");
        logger.info("Validating DNI: {} - Result: {}", dni, isValid);
        return isValid;
    }
}

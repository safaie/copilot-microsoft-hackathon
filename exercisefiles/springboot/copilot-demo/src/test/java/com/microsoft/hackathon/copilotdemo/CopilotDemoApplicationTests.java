package com.microsoft.hackathon.copilotdemo;

import com.microsoft.hackathon.copilotdemo.controller.DemoController;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.web.client.TestRestTemplate;
import org.springframework.boot.test.web.server.LocalServerPort;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders;
import org.springframework.test.web.servlet.result.MockMvcResultMatchers;
import org.springframework.http.ResponseEntity;

import static org.assertj.core.api.Assertions.assertThat;

@SpringBootTest(webEnvironment = SpringBootTest.WebEnvironment.RANDOM_PORT)
@AutoConfigureMockMvc 
class CopilotDemoApplicationTests {

    @LocalServerPort
    private int port;

    @Autowired
    private MockMvc mockMvc;

    @Autowired
    private TestRestTemplate restTemplate;

    @Autowired
    private DemoController controller;

    @Test
    void contextLoads() {
        assertThat(controller).isNotNull();
    }

    @Test
    void hello() throws Exception {
        mockMvc.perform(MockMvcRequestBuilders.get("/hello?key=world"))
            .andExpect(MockMvcResultMatchers.status().isOk())
            .andExpect(MockMvcResultMatchers.content().string("hello world"));
    }

    @Test
    void testHelloNoKey() {
        String url = "http://localhost:" + port + "/hello";
        ResponseEntity<String> response = restTemplate.getForEntity(url, String.class);
        assertThat(response.getStatusCodeValue()).isEqualTo(200);
        assertThat(response.getBody()).isEqualTo("key not passed");
    }

    @Test
    void testHelloWithKey() {
        String url = "http://localhost:" + port + "/hello?key=world";
        ResponseEntity<String> response = restTemplate.getForEntity(url, String.class);
        assertThat(response.getStatusCodeValue()).isEqualTo(200);
        assertThat(response.getBody()).isEqualTo("hello world");
    }

    @Test
    void testDiffDates() {
        String url = "http://localhost:" + port + "/diffdates?date1=01-01-2020&date2=10-01-2020";
        ResponseEntity<String> response = restTemplate.getForEntity(url, String.class);
        assertThat(response.getStatusCodeValue()).isEqualTo(200);
        assertThat(response.getBody()).isEqualTo("Difference in days: 9");
    }

    @Test
    void testValidatePhoneNumberValid() {
        String url = "http://localhost:" + port + "/validatePhoneNumber?phoneNumber=+34612345678";
        ResponseEntity<Boolean> response = restTemplate.getForEntity(url, Boolean.class);
        assertThat(response.getStatusCodeValue()).isEqualTo(200);
    }

}



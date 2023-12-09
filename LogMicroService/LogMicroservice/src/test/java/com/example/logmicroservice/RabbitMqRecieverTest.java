package com.example.logmicroservice;

import com.example.logmicroservice.DAO.Log;
import com.example.logmicroservice.DAO.LogRepository;
import com.example.logmicroservice.Service.RabbitmqReciever;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.TestPropertySource;

import java.util.List;

@SpringBootTest
@TestPropertySource(locations = "classpath:application-test.properties")
public class RabbitMqRecieverTest {
    @Autowired
    private RabbitmqReciever rabbitmqReceiver;

    @Autowired
    private LogRepository logRepository;

    @BeforeEach
    public void setUp() {
        // Drop the collection before each test
        logRepository.deleteAll();
    }

    @Test
    public void testReceiveMessage() {
        String message = "Test message";
        byte[] messageBytes = message.getBytes();

        rabbitmqReceiver.receiveMessage(messageBytes);

        // Retrieve the saved log from the repository
        Log savedLog = logRepository.findAll().get(0);

        // Assertions to verify that the message was saved correctly
        Assertions.assertEquals(message, savedLog.getMessage());
        Assertions.assertNotNull(savedLog.getId());
        Assertions.assertNotNull(savedLog.getTimestamp());
    }

    @Test
    public void testReceiveMessageEmpty() {
        String message = "";
        byte[] messageBytes = message.getBytes();

        rabbitmqReceiver.receiveMessage(messageBytes);

        // Retrieve the saved log from the repository
        List<Log> savedLog = logRepository.findAll();

        // Assertions to verify that the database is empty and that the message was not saved
        Assertions.assertEquals(0, logRepository.count());
        Assertions.assertEquals(0, savedLog.size());
    }
}

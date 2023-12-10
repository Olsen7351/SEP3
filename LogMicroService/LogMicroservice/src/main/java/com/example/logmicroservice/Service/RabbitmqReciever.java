package com.example.logmicroservice.Service;

import com.example.logmicroservice.DAO.Log;
import com.example.logmicroservice.DAO.LogRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;
import org.springframework.stereotype.Service;

import java.util.concurrent.CountDownLatch;

@Service
public class RabbitmqReciever {
    private CountDownLatch latch = new CountDownLatch(1);
    private final LogRepository logRepository;
    public RabbitmqReciever(@Autowired LogRepository logRepository) {
        this.logRepository = logRepository;
    }

    public void receiveMessage(byte[] message) {
        Log log = new Log(new String(message));
        log.setTimestamp(java.time.LocalDateTime.now());
        logRepository.save(log);
        System.out.println("Received <" + log.toString() + ">");
        latch.countDown();
    }

    public CountDownLatch getLatch() {
        return latch;
    }
}

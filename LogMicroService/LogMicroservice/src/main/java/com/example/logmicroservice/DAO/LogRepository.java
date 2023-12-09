package com.example.logmicroservice.DAO;

import org.springframework.data.mongodb.repository.MongoRepository;

public interface LogRepository extends MongoRepository<Log, Log> {
}

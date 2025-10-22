package com.fyno.api.routes.v1.integration.apikey.repository;

import com.fyno.api.routes.v1.integration.apikey.entity.ApiKey;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;
import java.util.Optional;

public interface ApiKeyRepository extends JpaRepository<ApiKey, String> {
    List<ApiKey> findByUserId(String userId);
    Optional<ApiKey> findByKeyValue(String keyValue);
    boolean existsByKeyValue(String keyValue);
}

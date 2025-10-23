package com.fyno.api.routes.v1.integration.apikey.repository;

import com.fyno.api.routes.v1.integration.apikey.entity.ApiKey;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface ApiKeyRepository extends JpaRepository<ApiKey, String> {
    List<ApiKey> findByUserId(String userId);
}

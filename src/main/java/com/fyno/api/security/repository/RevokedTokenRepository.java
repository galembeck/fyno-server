package com.fyno.api.security.repository;

import com.fyno.api.security.entity.RevokedToken;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;

public interface RevokedTokenRepository extends JpaRepository<RevokedToken, String> {
    boolean existsByToken(String token);
    Optional<RevokedToken> findByToken(String token);
}
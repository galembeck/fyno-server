package com.fyno.api.security.jwt;

import com.fyno.api.security.repository.RevokedTokenRepository;
import io.jsonwebtoken.Claims;
import io.jsonwebtoken.JwtException;
import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.SignatureAlgorithm;
import io.jsonwebtoken.security.Keys;
import org.springframework.stereotype.Service;

import java.security.Key;
import java.time.Instant;
import java.util.Date;

@Service
public class JwtService {

    private final RevokedTokenRepository revokedTokenRepository;

    public JwtService(RevokedTokenRepository revokedTokenRepository) {
        this.revokedTokenRepository = revokedTokenRepository;
    }

    private static final String SECRET_KEY =
            "f67d576745a2d48ed5460c774a29de5f212d9998745276de61a45970cd95690472ec412436d99e8aa7c6c4237e2e8961e6657cfaccd5ee60f6bf1ae7290d6e77";

    private Key getSigningKey() {
        return Keys.hmacShaKeyFor(SECRET_KEY.getBytes());
    }

    public String generateToken(String userId, String email, String role) {
        return Jwts.builder()
                .setSubject(userId)
                .claim("email", email)
                .claim("role", role)
                .setIssuedAt(new Date())
                .setExpiration(Date.from(Instant.now().plusSeconds(60 * 60 * 24))) // 24h
                .signWith(getSigningKey(), SignatureAlgorithm.HS256)
                .compact();
    }

    public Claims extractClaims(String token) {
        return Jwts.parser()
                .setSigningKey(getSigningKey())
                .build()
                .parseClaimsJws(token)
                .getBody();
    }

    public boolean isTokenValid(String token) {
        try {
            extractClaims(token);
            return !isTokenRevoked(token);
        } catch (JwtException e) {
            return false;
        }
    }

    public boolean isTokenRevoked(String token) {
        return revokedTokenRepository.existsByToken(token);
    }

    public void revokeToken(String token) {
        revokedTokenRepository.save(
                com.fyno.api.security.entity.RevokedToken.builder()
                        .token(token)
                        .revokedAt(Instant.now())
                        .build()
        );
    }
}

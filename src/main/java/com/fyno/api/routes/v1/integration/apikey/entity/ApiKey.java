package com.fyno.api.routes.v1.integration.apikey.entity;

import com.fyno.api.routes.user.entity.User;
import com.fyno.api.routes.v1.integration.apikey.enums.ApiKeyOrigin;
import jakarta.persistence.*;
import lombok.*;

import java.time.LocalDateTime;

@Entity
@Table(name = "api_keys")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class ApiKey {

    @Id
    @GeneratedValue(strategy = GenerationType.UUID)
    private String id;

    @Column(unique = true, nullable = false)
    private String keyValue;

    @Column(length = 255)
    private String notes;

    @Enumerated(EnumType.STRING)
    @Column(nullable = false)
    private ApiKeyOrigin origin;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "user_id", nullable = false)
    private User user;

    private boolean active = true;

    private LocalDateTime createdAt = LocalDateTime.now();
}

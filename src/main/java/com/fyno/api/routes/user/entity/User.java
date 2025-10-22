package com.fyno.api.routes.user.entity;

import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fyno.api.routes.user.enums.Role;
import jakarta.persistence.*;
import lombok.*;
import org.hibernate.annotations.CreationTimestamp;

import java.time.LocalDateTime;

@Entity
@Table(name = "users")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class User {
    @Id
    @GeneratedValue(strategy = GenerationType.UUID)
    private String id;

    private String email;
    @JsonIgnore
    private String password;

    private String name;
    private String lastname;
    private String phone;
    private String supportPhone;

    @Enumerated(EnumType.STRING)
    private Role role = Role.AUTHENTICATED;

    private String companyName;
    private String cnpj;
    private String monthlyRevenue;
    private String storeDomain;
    private String businessSegment;
    @Column(nullable = true)
    private String businessDescription;

    private String street;
    private String number;
    @Column(nullable = true)
    private String complement;
    private String neighborhood;
    private String cep;
    private String city;
    private String state;

    @CreationTimestamp
    @Column(updatable = false)
    private LocalDateTime createdAt;
}
package com.fyno.api.routes.auth.dto;

public record RegisterUserDTO(
        String email,
        String password,

        String name,
        String lastname,
        String phone,
        String supportPhone,
        String companyName,

        String cnpj,
        String monthlyRevenue,
        String storeDomain,
        String businessSegment,
        String businessDescription,

        String street,
        String number,
        String complement,
        String neighborhood,
        String cep,
        String city,
        String state
) {}

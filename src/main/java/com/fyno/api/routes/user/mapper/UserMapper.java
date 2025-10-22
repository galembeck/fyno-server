package com.fyno.api.routes.user.mapper;

import com.fyno.api.routes.auth.dto.RegisterUserDTO;
import com.fyno.api.routes.user.entity.User;
import com.fyno.api.routes.user.enums.Role;

public class UserMapper {
    public static User toEntity(RegisterUserDTO dto) {
        User user = new User();
        user.setName(dto.name());
        user.setLastname(dto.lastname());
        user.setEmail(dto.email());
        user.setPhone(dto.phone());
        user.setSupportPhone(dto.supportPhone());
        user.setPassword(dto.password());
        user.setRole(Role.AUTHENTICATED);
        user.setCompanyName(dto.companyName());
        user.setCnpj(dto.cnpj());
        user.setMonthlyRevenue(dto.monthlyRevenue());
        user.setStoreDomain(dto.storeDomain());
        user.setBusinessSegment(dto.businessSegment());
        user.setBusinessDescription(dto.businessDescription());
        user.setStreet(dto.street());
        user.setNumber(dto.number());
        user.setComplement(dto.complement());
        user.setNeighborhood(dto.neighborhood());
        user.setCep(dto.cep());
        user.setCity(dto.city());
        user.setState(dto.state());
        return user;
    }
}
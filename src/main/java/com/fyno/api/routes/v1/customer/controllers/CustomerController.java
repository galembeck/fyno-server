package com.fyno.api.routes.v1.customer.controllers;

import com.fyno.api.common.ApiResponse;
import com.fyno.api.routes.v1.customer.dto.request.CreateCustomerDTO;
import com.fyno.api.routes.v1.customer.services.CustomerService;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
import jakarta.servlet.http.HttpServletRequest;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/v1/customer")
@Tag(name = "Customer", description = "Customers/clients management")
public class CustomerController {

    private final CustomerService service;

    public CustomerController(CustomerService service) {
        this.service = service;
    }

    @Operation(summary = "Create a new customer/client")
    @PostMapping("/create")
    public ResponseEntity<ApiResponse<?>> create(
            @RequestBody CreateCustomerDTO dto,
            HttpServletRequest req
    ) {
        var created = service.createCustomer(dto);

        return ResponseEntity.ok(ApiResponse.ok(created, req.getRequestURI(), null));
    }

    @Operation(summary = "List all customers/clients")
    @PostMapping("/list")
    public ResponseEntity<ApiResponse<?>> list(HttpServletRequest req) {
        var customers = service.listCustomers();

        return ResponseEntity.ok(ApiResponse.ok(customers, req.getRequestURI(), null));
    }
}

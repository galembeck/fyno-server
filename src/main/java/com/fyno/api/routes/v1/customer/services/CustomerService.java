package com.fyno.api.routes.v1.customer.services;

import com.fyno.api.exception.ApiException;
import com.fyno.api.exception.ErrorCodes;
import com.fyno.api.routes.v1.customer.dto.request.CreateCustomerDTO;
import com.fyno.api.routes.v1.customer.dto.response.CustomerResponseDTO;
import com.fyno.api.routes.v1.customer.entities.Customer;
import com.fyno.api.routes.v1.customer.repository.CustomerRepository;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class CustomerService {

    private final CustomerRepository repository;

    public CustomerService(CustomerRepository repository) {
        this.repository = repository;
    }

    public CustomerResponseDTO createCustomer(CreateCustomerDTO dto) {
        if (repository.existsByEmail(dto.email())) {
            throw ApiException.of(ErrorCodes.USER_ALREADY_REGISTERED);
        }

        var existingCustomer = repository.findByDocument(dto.document());
        if (existingCustomer.isPresent()) {
            var customer = existingCustomer.get();
            return new CustomerResponseDTO(
                    customer.getId(),
                    customer.getName(),
                    customer.getEmail(),
                    customer.getPhone(),
                    customer.getDocument(),
                    customer.getAddress(),
                    customer.getCreatedAt()
            );
        }

        Customer customer = Customer.builder()
                .name(dto.name())
                .email(dto.email())
                .phone(dto.phone())
                .document(dto.document())
                .address(dto.address())
                .build();

        repository.save(customer);

        return new CustomerResponseDTO(
                customer.getId(),
                customer.getName(),
                customer.getEmail(),
                customer.getPhone(),
                customer.getDocument(),
                customer.getAddress(),
                customer.getCreatedAt()
        );
    }

    public List<CustomerResponseDTO> listCustomers() {
        return repository.findAll().stream()
                .map(customer -> new CustomerResponseDTO(
                        customer.getId(),
                        customer.getName(),
                        customer.getEmail(),
                        customer.getPhone(),
                        customer.getDocument(),
                        customer.getAddress(),
                        customer.getCreatedAt()
                ))
                .toList();
    }
}
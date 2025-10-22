package com.fyno.api.routes.v1.customer.repository;

import com.fyno.api.routes.v1.customer.entities.Customer;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface CustomerRepository extends JpaRepository<Customer, String> {
    Optional<Customer> findByEmail(String email);
    Optional<Customer> findByDocument(String document);
    boolean existsByEmail(String email);
    boolean existsByDocument(String document);
}

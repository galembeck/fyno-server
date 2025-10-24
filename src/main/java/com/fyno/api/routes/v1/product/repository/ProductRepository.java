package com.fyno.api.routes.v1.product.repository;

import com.fyno.api.routes.v1.product.entity.Product;
import org.springframework.data.jpa.repository.JpaRepository;

public interface ProductRepository extends JpaRepository<Product, String> {}

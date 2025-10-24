package com.fyno.api.routes.v1.product.service;

import com.fyno.api.routes.v1.product.dto.request.ProductRequestDTO;
import com.fyno.api.routes.v1.product.dto.response.ProductResponseDTO;

import java.util.List;

public interface ProductService {
    ProductResponseDTO createProduct(ProductRequestDTO dto);
    List<ProductResponseDTO> listProducts();
    ProductResponseDTO updateProduct(String id, ProductRequestDTO dto);
    void deleteProduct(String id);
}

package com.fyno.api.routes.v1.product.service.impl;

import com.fyno.api.exception.ApiException;
import com.fyno.api.exception.ErrorCodes;
import com.fyno.api.routes.v1.product.dto.request.ProductRequestDTO;
import com.fyno.api.routes.v1.product.dto.response.ProductResponseDTO;
import com.fyno.api.routes.v1.product.entity.Product;
import com.fyno.api.routes.v1.product.repository.ProductRepository;
import com.fyno.api.routes.v1.product.service.ProductService;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.stream.Collectors;

@Service
public class ProductServiceImpl implements ProductService {

    private final ProductRepository repository;

    public ProductServiceImpl(ProductRepository repository) {
        this.repository = repository;
    }

    @Override
    public ProductResponseDTO createProduct(ProductRequestDTO dto) {
        if (repository.existsById(dto.id())) {
            throw ApiException.of(ErrorCodes.DATA_INTEGRITY_VIOLATION);
        }

        Product product = Product.builder()
                .id(dto.id())
                .name(dto.name())
                .description(dto.description())
                .price(dto.price())
                .build();

        repository.save(product);

        return new ProductResponseDTO(
                product.getId(),
                product.getName(),
                product.getDescription(),
                product.getPrice(),
                product.getCreatedAt()
        );
    }

    @Override
    public List<ProductResponseDTO> listProducts() {
        return repository.findAll().stream()
                .map(p -> new ProductResponseDTO(
                        p.getId(),
                        p.getName(),
                        p.getDescription(),
                        p.getPrice(),
                        p.getCreatedAt()
                ))
                .collect(Collectors.toList());
    }

    @Override
    public ProductResponseDTO updateProduct(String id, ProductRequestDTO dto) {
        Product product = repository.findById(id)
                .orElseThrow(() -> ApiException.of(ErrorCodes.PRODUCT_NOT_FOUND));

        product.setName(dto.name());
        product.setDescription(dto.description());
        product.setPrice(dto.price());

        repository.save(product);

        return new ProductResponseDTO(
                product.getId(),
                product.getName(),
                product.getDescription(),
                product.getPrice(),
                product.getCreatedAt()
        );
    }

    @Override
    public void deleteProduct(String id) {
        if (!repository.existsById(id)) {
            throw ApiException.of(ErrorCodes.PRODUCT_NOT_FOUND);
        }
        repository.deleteById(id);
    }
}

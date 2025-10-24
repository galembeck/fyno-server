package com.fyno.api.routes.v1.product.controller;

import com.fyno.api.common.ApiResponse;
import com.fyno.api.routes.v1.product.dto.request.ProductRequestDTO;
import com.fyno.api.routes.v1.product.dto.response.ProductResponseDTO;
import com.fyno.api.routes.v1.product.service.ProductService;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import jakarta.servlet.http.HttpServletRequest;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/v1/product")
public class ProductController {

    private final ProductService service;

    public ProductController(ProductService service) {
        this.service = service;
    }

    @Operation(summary = "Create new product", description = "Register a new product in the system")
    @ApiResponses({
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "200", description = "Product created successfully"),
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "401", description = "Unauthorized")
    })
    @PostMapping("/create")
    public ApiResponse<ProductResponseDTO> create(
            @RequestBody ProductRequestDTO dto,
            HttpServletRequest req
    ) {
        var result = service.createProduct(dto);
        return ApiResponse.ok(result, req.getRequestURI(), null);
    }

    @Operation(summary = "List products", description = "Return all products registered in the system")
    @GetMapping("/list")
    public ApiResponse<List<ProductResponseDTO>> list(HttpServletRequest req) {
        var result = service.listProducts();
        return ApiResponse.ok(result, req.getRequestURI(), null);
    }

    @Operation(summary = "Update product", description = "Update an existing product by its ID")
    @PutMapping("/update/{id}")
    public ApiResponse<ProductResponseDTO> update(
            @PathVariable String id,
            @RequestBody ProductRequestDTO dto,
            HttpServletRequest req
    ) {
        var result = service.updateProduct(id, dto);
        return ApiResponse.ok(result, req.getRequestURI(), null);
    }

    @Operation(summary = "Delete product", description = "Remove a product from the system by its ID")
    @DeleteMapping("/delete/{id}")
    public ApiResponse<Void> delete(
            @PathVariable String id,
            HttpServletRequest req
    ) {
        service.deleteProduct(id);
        return ApiResponse.ok(null, req.getRequestURI(), null);
    }
}

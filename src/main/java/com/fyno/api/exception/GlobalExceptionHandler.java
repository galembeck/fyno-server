package com.fyno.api.exception;

import com.fyno.api.common.ApiResponse;
import jakarta.servlet.http.HttpServletRequest;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.http.converter.HttpMessageNotReadableException;
import org.springframework.web.bind.MethodArgumentNotValidException;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;

import java.util.HashMap;
import java.util.Map;

@ControllerAdvice
public class GlobalExceptionHandler {

    private String rid(HttpServletRequest req) {
        return req.getHeader("X-Request-Id");
    }

    @ExceptionHandler(ApiException.class)
    public ResponseEntity<ApiResponse<?>> handleApi(ApiException ex, HttpServletRequest req) {
        var error = ApiResponse.ApiError.builder()
                .code(ex.getCode())
                .message(ex.getMessage())
                .build();

        return ResponseEntity
                .status(ex.getStatus())
                .body(ApiResponse.fail(error, req.getRequestURI(), rid(req)));
    }

    @ExceptionHandler(MethodArgumentNotValidException.class)
    public ResponseEntity<ApiResponse<?>> handleValidation(MethodArgumentNotValidException ex, HttpServletRequest req) {
        Map<String, Object> details = new HashMap<>();
        ex.getBindingResult().getFieldErrors().forEach(fe ->
                details.put(fe.getField(), fe.getDefaultMessage())
        );

        var error = ApiResponse.ApiError.builder()
                .code("VALIDATION_ERROR")
                .message("Invalid request parameters")
                .details(details)
                .build();

        return ResponseEntity.badRequest()
                .body(ApiResponse.fail(error, req.getRequestURI(), rid(req)));
    }

    @ExceptionHandler(DataIntegrityViolationException.class)
    public ResponseEntity<ApiResponse<?>> handleDataIntegrity(DataIntegrityViolationException ex, HttpServletRequest req) {
        var error = ApiResponse.ApiError.builder()
                .code("DATA_INTEGRITY_VIOLATION")
                .message("Violation of data integrity constraints")
                .build();

        return ResponseEntity.status(HttpStatus.CONFLICT)
                .body(ApiResponse.fail(error, req.getRequestURI(), rid(req)));
    }

    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class)
    public ResponseEntity<ApiResponse<?>> handleAccessDenied(org.springframework.security.access.AccessDeniedException ex, HttpServletRequest req) {
        var error = ApiResponse.ApiError.builder()
                .code("FORBIDDEN")
                .message("Access denied/forbidden")
                .build();

        return ResponseEntity.status(HttpStatus.FORBIDDEN)
                .body(ApiResponse.fail(error, req.getRequestURI(), rid(req)));
    }

    @ExceptionHandler(HttpMessageNotReadableException.class)
    public ResponseEntity<ApiResponse<?>> handleDeserialization(HttpMessageNotReadableException ex, HttpServletRequest req) {
        var error = ApiResponse.ApiError.builder()
                .code("BAD_REQUEST")
                .message("Erro de leitura do corpo da requisição: " + ex.getMostSpecificCause().getMessage())
                .build();
        return ResponseEntity
                .status(HttpStatus.BAD_REQUEST)
                .body(ApiResponse.fail(error, req.getRequestURI(), rid(req)));
    }

    @ExceptionHandler(Exception.class)
    public ResponseEntity<ApiResponse<?>> handleGeneric(Exception ex, HttpServletRequest req) {
        var error = ApiResponse.ApiError.builder()
                .code("INTERNAL_ERROR")
                .message("Internal server error")
                .build();

        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                .body(ApiResponse.fail(error, req.getRequestURI(), rid(req)));
    }
}
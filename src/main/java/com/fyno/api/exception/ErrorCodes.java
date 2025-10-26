package com.fyno.api.exception;

import org.springframework.http.HttpStatus;

public enum ErrorCodes {
    // AUTH ERRORS
    USER_ALREADY_REGISTERED("USER_ALREADY_REGISTERED", "E-mail already registered", HttpStatus.CONFLICT),
    USER_NOT_FOUND("USER_NOT_FOUND", "User not found", HttpStatus.NOT_FOUND),
    INVALID_PASSWORD("INVALID_PASSWORD", "Incorrect password", HttpStatus.UNAUTHORIZED),
    TOKEN_INVALID("TOKEN_INVALID", "Invalid JWT token", HttpStatus.UNAUTHORIZED),
    TOKEN_EXPIRED("TOKEN_EXPIRED", "Expired JWT token", HttpStatus.UNAUTHORIZED),
    TOKEN_REVOKED("TOKEN_REVOKED", "Token revoked, login again", HttpStatus.UNAUTHORIZED),

    // SYSTEM / DATABASE
    DATABASE_ERROR("DATABASE_ERROR", "Database error", HttpStatus.INTERNAL_SERVER_ERROR),
    INTERNAL_SERVER_ERROR("INTERNAL_SERVER_ERROR", "Internal server error", HttpStatus.INTERNAL_SERVER_ERROR),
    DATA_INTEGRITY_VIOLATION("DATA_INTEGRITY_VIOLATION", "Data integrity violation", HttpStatus.CONFLICT),

    // PERMISSION / ACCESS
    FORBIDDEN("FORBIDDEN", "Access denied/forbidden", HttpStatus.FORBIDDEN),
    UNAUTHORIZED("UNAUTHORIZED", "Unauthorized", HttpStatus.UNAUTHORIZED),

    // INTEGRATION
    API_KEY_NOT_FOUND("API_KEY_NOT_FOUND", "API key not found", HttpStatus.NOT_FOUND),
    WEBHOOK_NOT_FOUND("WEBHOOK_NOT_FOUND", "Webhook not found", HttpStatus.NOT_FOUND),

    PRODUCT_NOT_FOUND("PRODUCT_NOT_FOUND", "Product not found", HttpStatus.NOT_FOUND),
    RESOURCE_NOT_FOUND("RESOURCE_NOT_FOUND", "Requested resource not found", HttpStatus.NOT_FOUND);

    private final String code;
    private final String message;
    private final HttpStatus status;

    ErrorCodes(String code, String message, HttpStatus status) {
        this.code = code;
        this.message = message;
        this.status = status;
    }

    public String code() { return code; }
    public String message() { return message; }
    public HttpStatus status() { return status; }
}

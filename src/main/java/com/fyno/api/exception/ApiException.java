package com.fyno.api.exception;

import org.springframework.http.HttpStatus;

public class ApiException extends RuntimeException {
    private final ErrorCodes errorCode;

    public ApiException(ErrorCodes errorCode) {
        super(errorCode.message());
        this.errorCode = errorCode;
    }

    public HttpStatus getStatus() { return errorCode.status(); }
    public String getCode() { return errorCode.code(); }
    public ErrorCodes getErrorCode() { return errorCode; }

    public static ApiException of(ErrorCodes code) {
        return new ApiException(code);
    }
}
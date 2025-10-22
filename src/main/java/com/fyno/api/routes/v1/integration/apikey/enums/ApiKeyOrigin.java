package com.fyno.api.routes.v1.integration.apikey.enums;

import com.fasterxml.jackson.annotation.JsonCreator;

public enum ApiKeyOrigin {
    DASHBOARD;

    @JsonCreator
    public static ApiKeyOrigin fromString(String value) {
        if (value == null) return null;
        return ApiKeyOrigin.valueOf(value.trim().toUpperCase());
    }
}

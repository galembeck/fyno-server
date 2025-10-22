package com.fyno.api.config;

import io.swagger.v3.oas.models.Components;
import io.swagger.v3.oas.models.OpenAPI;
import io.swagger.v3.oas.models.info.Contact;
import io.swagger.v3.oas.models.info.Info;
import io.swagger.v3.oas.models.security.SecurityRequirement;
import io.swagger.v3.oas.models.security.SecurityScheme;
import io.swagger.v3.oas.models.servers.Server;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import java.util.List;

@Configuration
public class OpenApiConfig {

    @Bean
    public OpenAPI fynoOpenAPI() {
        final String schemeName = "BearerAuth";
        final String bearerFormat = "JWT";

        return new OpenAPI()
                .info(new Info()
                        .title("Fyno API")
                        .version("v1.0.0")
                        .description("Documentation of Fyno API for developers integration purposes")
                        .contact(new Contact()
                                .name("Fyno")
                                .email("suporte@fyno.dev")))
//                                .url("https://fyno.dev")))
                .addSecurityItem(new SecurityRequirement().addList(schemeName))
                .components(new Components().addSecuritySchemes(schemeName,
                        new SecurityScheme()
                                .name(schemeName)
                                .type(SecurityScheme.Type.HTTP)
                                .scheme("bearer")
                                .bearerFormat(bearerFormat)))
                .servers(List.of(
                        new Server().url("http://localhost:8080").description("Fyno API | Development Server"),
                        new Server().url("https://api.fyno.dev").description("Fyno API | Production Server")
                ));
    }
}
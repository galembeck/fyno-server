package com.fyno.api.routes.roadmap.suggestions.dto.response;

public record RoadmapSuggestionResponseDTO(
        String id,
        String title,
        String description,
        int votes
) {}

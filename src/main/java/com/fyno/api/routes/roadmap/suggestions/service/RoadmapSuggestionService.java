package com.fyno.api.routes.roadmap.suggestions.service;

import com.fyno.api.routes.roadmap.suggestions.dto.request.RoadmapSuggestionRequestDTO;
import com.fyno.api.routes.roadmap.suggestions.dto.response.RoadmapSuggestionResponseDTO;

import java.util.List;

public interface RoadmapSuggestionService {
    RoadmapSuggestionResponseDTO createSuggestion(RoadmapSuggestionRequestDTO dto);
    List<RoadmapSuggestionResponseDTO> listSuggestions();
}

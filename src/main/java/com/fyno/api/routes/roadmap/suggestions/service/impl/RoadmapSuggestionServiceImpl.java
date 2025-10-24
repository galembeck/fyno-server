package com.fyno.api.routes.roadmap.suggestions.service.impl;

import com.fyno.api.routes.roadmap.suggestions.dto.request.RoadmapSuggestionRequestDTO;
import com.fyno.api.routes.roadmap.suggestions.dto.response.RoadmapSuggestionResponseDTO;
import com.fyno.api.routes.roadmap.suggestions.entity.RoadmapSuggestion;
import com.fyno.api.routes.roadmap.suggestions.repository.RoadmapSuggestionRepository;
import com.fyno.api.routes.roadmap.suggestions.service.RoadmapSuggestionService;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class RoadmapSuggestionServiceImpl implements RoadmapSuggestionService {

    private final RoadmapSuggestionRepository repository;

    public RoadmapSuggestionServiceImpl(RoadmapSuggestionRepository repository) {
        this.repository = repository;
    }

    @Override
    public RoadmapSuggestionResponseDTO createSuggestion(RoadmapSuggestionRequestDTO dto) {
        RoadmapSuggestion suggestion = RoadmapSuggestion.builder()
                .title(dto.title())
                .description(dto.description())
                .votes(0)
                .build();

        repository.save(suggestion);

        return new RoadmapSuggestionResponseDTO(
                suggestion.getId(),
                suggestion.getTitle(),
                suggestion.getDescription(),
                suggestion.getVotes()
        );
    }

    @Override
    public List<RoadmapSuggestionResponseDTO> listSuggestions() {
        return repository.findAll()
                .stream()
                .map(s -> new RoadmapSuggestionResponseDTO(
                        s.getId(),
                        s.getTitle(),
                        s.getDescription(),
                        s.getVotes()
                ))
                .toList();
    }
}

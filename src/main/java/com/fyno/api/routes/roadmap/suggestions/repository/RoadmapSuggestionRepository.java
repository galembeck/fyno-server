package com.fyno.api.routes.roadmap.suggestions.repository;

import com.fyno.api.routes.roadmap.suggestions.entity.RoadmapSuggestion;
import org.springframework.data.jpa.repository.JpaRepository;

public interface RoadmapSuggestionRepository extends JpaRepository<RoadmapSuggestion, String> {}

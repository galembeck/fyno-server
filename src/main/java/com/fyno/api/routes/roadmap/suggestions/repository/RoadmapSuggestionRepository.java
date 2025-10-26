package com.fyno.api.routes.roadmap.suggestions.repository;

import com.fyno.api.routes.roadmap.suggestions.entity.RoadmapSuggestion;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import java.util.List;

public interface RoadmapSuggestionRepository extends JpaRepository<RoadmapSuggestion, String> {
    @Query("SELECT s FROM RoadmapSuggestion s ORDER BY s.createdAt ASC")
    List<RoadmapSuggestion> findAllOrdered();
}

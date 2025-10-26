package com.fyno.api.routes.roadmap.suggestions.repository;

import com.fyno.api.routes.roadmap.suggestions.entity.RoadmapVote;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;

public interface RoadmapVoteRepository extends JpaRepository<RoadmapVote, String> {
    Optional<RoadmapVote> findByUserIdAndSuggestionId(String userId, String suggestionId);
    long countBySuggestionId(String suggestionId);
}

package com.fyno.api.routes.roadmap.suggestions.service;

public interface RoadmapVoteService {
    int toggleVote(String userEmail, String suggestionId);
}


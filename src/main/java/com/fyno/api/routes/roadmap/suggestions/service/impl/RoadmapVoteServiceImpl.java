package com.fyno.api.routes.roadmap.suggestions.service.impl;

import com.fyno.api.exception.ApiException;
import com.fyno.api.exception.ErrorCodes;
import com.fyno.api.routes.roadmap.suggestions.entity.RoadmapSuggestion;
import com.fyno.api.routes.roadmap.suggestions.entity.RoadmapVote;
import com.fyno.api.routes.roadmap.suggestions.repository.RoadmapSuggestionRepository;
import com.fyno.api.routes.roadmap.suggestions.repository.RoadmapVoteRepository;
import com.fyno.api.routes.roadmap.suggestions.service.RoadmapVoteService;
import com.fyno.api.routes.user.entity.User;
import com.fyno.api.routes.user.repository.UserRepository;
import jakarta.transaction.Transactional;
import org.springframework.stereotype.Service;

@Service
public class RoadmapVoteServiceImpl implements RoadmapVoteService {

    private final RoadmapVoteRepository voteRepository;
    private final RoadmapSuggestionRepository suggestionRepository;
    private final UserRepository userRepository;

    public RoadmapVoteServiceImpl(
            RoadmapVoteRepository voteRepository,
            RoadmapSuggestionRepository suggestionRepository,
            UserRepository userRepository
    ) {
        this.voteRepository = voteRepository;
        this.suggestionRepository = suggestionRepository;
        this.userRepository = userRepository;
    }

    @Override
    @Transactional
    public int toggleVote(String userEmail, String suggestionId) {
        User user = userRepository.findByEmail(userEmail)
                .orElseThrow(() -> ApiException.of(ErrorCodes.USER_NOT_FOUND));

        RoadmapSuggestion suggestion = suggestionRepository.findById(suggestionId)
                .orElseThrow(() -> ApiException.of(ErrorCodes.RESOURCE_NOT_FOUND));

        var existingVote = voteRepository.findByUserIdAndSuggestionId(user.getId(), suggestion.getId());

        if (existingVote.isPresent()) {
            voteRepository.delete(existingVote.get());
            suggestion.setVotes(Math.max(0, suggestion.getVotes() - 1));
        } else {
            RoadmapVote vote = RoadmapVote.builder()
                    .user(user)
                    .suggestion(suggestion)
                    .build();
            voteRepository.save(vote);
            suggestion.setVotes(suggestion.getVotes() + 1);
        }

        suggestionRepository.save(suggestion);
        return suggestion.getVotes();
    }
}

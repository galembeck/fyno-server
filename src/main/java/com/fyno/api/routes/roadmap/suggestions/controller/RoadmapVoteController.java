package com.fyno.api.routes.roadmap.suggestions.controller;

import com.fyno.api.common.ApiResponse;
import com.fyno.api.exception.ApiException;
import com.fyno.api.exception.ErrorCodes;
import com.fyno.api.routes.roadmap.suggestions.service.RoadmapVoteService;
import com.fyno.api.security.entity.AuthenticatedUser;
import com.fyno.api.security.entity.CurrentUser;
import io.swagger.v3.oas.annotations.Operation;
import jakarta.servlet.http.HttpServletRequest;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/roadmap")
public class RoadmapVoteController {

    private final RoadmapVoteService service;

    public RoadmapVoteController(RoadmapVoteService service) {
        this.service = service;
    }

    @Operation(summary = "Vote or unvote a suggestion")
    @PostMapping("/vote/{suggestionId}")
    public ApiResponse<Integer> toggleVote(
            @CurrentUser AuthenticatedUser user,
            @PathVariable String suggestionId,
            HttpServletRequest req
    ) {
        if (user == null) throw ApiException.of(ErrorCodes.UNAUTHORIZED);
        int updatedVotes = service.toggleVote(user.getEmail(), suggestionId);
        return ApiResponse.ok(updatedVotes, req.getRequestURI(), null);
    }
}
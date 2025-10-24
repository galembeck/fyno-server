package com.fyno.api.routes.roadmap.suggestions.controller;

import com.fyno.api.common.ApiResponse;
import com.fyno.api.routes.roadmap.suggestions.dto.request.RoadmapSuggestionRequestDTO;
import com.fyno.api.routes.roadmap.suggestions.dto.response.RoadmapSuggestionResponseDTO;
import com.fyno.api.routes.roadmap.suggestions.service.RoadmapSuggestionService;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import jakarta.servlet.http.HttpServletRequest;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/roadmap")
public class RoadmapSuggestionController {

    private final RoadmapSuggestionService service;

    public RoadmapSuggestionController(RoadmapSuggestionService service) {
        this.service = service;
    }

    @Operation(summary = "Create new suggestion", description = "Register a new suggestion to the roadmap")
    @ApiResponses({
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "200", description = "Suggestion created successfully"),
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "401", description = "Unauthorized")
    })
    @PostMapping("/create")
    public ApiResponse<RoadmapSuggestionResponseDTO> create(
            @RequestBody RoadmapSuggestionRequestDTO dto,
            HttpServletRequest req
    ) {
        var result = service.createSuggestion(dto);
        return ApiResponse.ok(result, req.getRequestURI(), null);
    }

    @Operation(summary = "List all roadmap suggestions", description = "Return all suggestions registered in the roadmap")
    @GetMapping("/list")
    public ApiResponse<List<RoadmapSuggestionResponseDTO>> list(HttpServletRequest req) {
        var result = service.listSuggestions();
        return ApiResponse.ok(result, req.getRequestURI(), null);
    }
}

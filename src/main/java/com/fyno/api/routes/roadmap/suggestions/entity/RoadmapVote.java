package com.fyno.api.routes.roadmap.suggestions.entity;

import com.fyno.api.routes.user.entity.User;
import jakarta.persistence.*;
import lombok.*;

@Entity
@Table(name = "roadmap_votes", uniqueConstraints = {
        @UniqueConstraint(columnNames = {"user_id", "suggestion_id"})
})
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class RoadmapVote {

    @Id
    @GeneratedValue(strategy = GenerationType.UUID)
    private String id;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "user_id", nullable = false)
    private User user;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "suggestion_id", nullable = false)
    private RoadmapSuggestion suggestion;
}

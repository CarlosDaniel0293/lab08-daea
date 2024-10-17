CREATE TABLE IF NOT EXISTS votes (
    id SERIAL PRIMARY KEY,
    option1_votes INT DEFAULT 0,
    option2_votes INT DEFAULT 0
);

INSERT INTO votes (id, option1_votes, option2_votes) VALUES (1, 0, 0) ON CONFLICT (id) DO NOTHING;

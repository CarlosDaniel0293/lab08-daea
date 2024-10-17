const express = require('express');
const { Pool } = require('pg');

const app = express();
const port = 3000;

const pool = new Pool({
    host: 'postgres',
    port: 5432,
    user: 'your_user',        
    password: 'your_password', 
    database: 'your_database'  
});

app.get('/votes', async (req, res) => {
    try {
        const result = await pool.query('SELECT * FROM votes WHERE id = 1');
        res.json(result.rows[0]);
    } catch (err) {
        console.error(err);
        res.status(500).send('Error al obtener los votos');
    }
});

app.listen(port, () => {
    console.log(`Server running at http://localhost:${port}`);
});

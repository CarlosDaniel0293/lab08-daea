from flask import Flask, render_template, request, jsonify
import redis
import logging

app = Flask(__name__)

# Configurar el registro de depuración
logging.basicConfig(level=logging.DEBUG)

# Conectar con Redis
r = redis.StrictRedis(host='redis', port=6379, db=0, decode_responses=True)

@app.route('/')
def index():
    # Renderizar el frontend de votación (index.html)
    return render_template('index.html')

@app.route('/vote', methods=['POST'])
def vote():
    vote = request.form.get('vote')
    app.logger.debug(f"Received vote: {vote}")
    if vote:
        r.incr(vote)  # Incrementar el conteo del voto en Redis
        return jsonify({"status": "success", "vote": vote})
    else:
        app.logger.debug("No vote received.")
        return jsonify({"status": "error", "message": "No vote received"}), 400

@app.route('/results', methods=['GET'])
def results():
    votes = {key: r.get(key) for key in r.keys('*')}
    return jsonify(votes)

if __name__ == "__main__":
    app.run(host='0.0.0.0')

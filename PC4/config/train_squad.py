# train_squad.py

# Configuración para entrenar tu propio modelo con tus datos tokenizados

out_dir = 'out-squad'  # Cambia esto al nombre de tu carpeta de salida
eval_interval = 250  # Intervalo de evaluación
eval_iters = 200
log_interval = 10  # Intervalo de log

# Guardar checkpoints solo cuando mejora la validación
always_save_checkpoint = False

wandb_log = False  # Puedes habilitarlo si usas wandb
wandb_project = 'squad'
wandb_run_name = 'mini-gpt'

dataset = 'squad'  # Nombre de tu dataset
gradient_accumulation_steps = 4
batch_size = 64
block_size = 512  # Ajusta según tu dataset

# Configuración del modelo
n_layer = 12
n_head = 12
n_embd = 768
dropout = 0.2

learning_rate = 5e-4  # Puedes ajustar esto según sea necesario
max_iters = 20000
lr_decay_iters = 20000  # Generalmente igual a max_iters
min_lr = 5e-5  # learning_rate / 10 generalmente
beta1 = 0.9
beta2 = 0.98

warmup_iters = 1000   # No es super necesario potencialmente

# Opcional: Ejecutar en CPU y sin compilación (útil para depuración)
# device = 'cpu'
# compile = False

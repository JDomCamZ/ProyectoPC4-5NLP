import os
import torch
from datasets import load_dataset
from transformers import GPT2Tokenizer, GPT2LMHeadModel

# Cargar el tokenizador de GPT-2
tokenizer = GPT2Tokenizer.from_pretrained('gpt2')

# Cargar el modelo desde el checkpoint guardado localmente
model_path = 'out-squad/ckpt.pt'
checkpoint = torch.load(model_path)
model = GPT2LMHeadModel.from_pretrained('gpt2')
model.load_state_dict(checkpoint['model'])

model.eval()
device = 'cuda' if torch.cuda.is_available() else 'cpu'
model.to(device)

# Cargar el dataset SQuAD
dataset = load_dataset("squad")

def evaluate_model(model, dataset, tokenizer, device):
    total_examples = 0
    exact_matches = 0
    f1_sum = 0

    for example in dataset['validation']:
        context = example['context']
        question = example['question']
        answers = example['answers']['text']

        # Preprocesar entrada
        input_text = f"Question: {question} Context: {context} Answer:"
        inputs = tokenizer(input_text, return_tensors='pt').to(device)
        
        # Generar respuesta
        with torch.no_grad():
            outputs = model.generate(inputs['input_ids'], max_length=512)
        
        generated_answer = tokenizer.decode(outputs[0], skip_special_tokens=True)

        # Evaluar la respuesta generada
        exact_match = max([int(generated_answer.strip() == answer) for answer in answers])
        exact_matches += exact_match

        f1 = max([compute_f1(generated_answer.strip(), answer) for answer in answers])
        f1_sum += f1

        total_examples += 1

    em = exact_matches / total_examples
    f1 = f1_sum / total_examples

    return em, f1

def compute_f1(pred, truth):
    pred_tokens = pred.split()
    truth_tokens = truth.split()
    
    common = set(pred_tokens) & set(truth_tokens)
    num_common = len(common)
    
    if num_common == 0:
        return 0
    
    precision = num_common / len(pred_tokens)
    recall = num_common / len(truth_tokens)
    
    f1 = 2 * (precision * recall) / (precision + recall)
    return f1

em, f1 = evaluate_model(model, dataset, tokenizer, device)
print(f"Exact Match: {em}")
print(f"F1 Score: {f1}")

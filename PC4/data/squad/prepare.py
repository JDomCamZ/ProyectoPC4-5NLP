import os
from tqdm import tqdm
import numpy as np
import tiktoken
import pickle
from datasets import load_dataset # huggingface datasets

# number of workers in .map() call
num_proc = 8

# number of workers in load_dataset() call
num_proc_load_dataset = num_proc

enc = tiktoken.get_encoding("gpt2")

if __name__ == '__main__':
    # cargar el dataset de SQuAD
    dataset = load_dataset("knowledgator/Scientific-text-classificationsquad", num_proc=num_proc_load_dataset)
    print(dataset)

    # SQuAD tiene 'train' y 'validation' splits, renombrar validation a val
    #dataset = dataset.rename_column("validation", "val")

    #print(dataset)

    # definir la función de tokenización
    def process(example):
        ids = enc.encode_ordinary(example['context']) # encode_ordinary ignores any special tokens
        ids.append(enc.eot_token) # add the end of text token, e.g. 50256 for gpt2 bpe
        out = {'ids': ids, 'len': len(ids)}
        return out

    # tokenizar el dataset
    tokenized = dataset.map(
        process,
        remove_columns=['context'],
        desc="tokenizing the splits",
        num_proc=num_proc,
    )

    # concatenar todos los ids en cada split en un archivo grande para entrenamiento
    for split, dset in tokenized.items():
        arr_len = np.sum(dset['len'], dtype=np.uint64)
        filename = os.path.join(os.path.dirname(__file__), f'{split}.bin')
        dtype = np.uint16 # (can do since enc.max_token_value == 50256 is < 2**16)
        arr = np.memmap(filename, dtype=dtype, mode='w+', shape=(arr_len,))
        total_batches = 1024

        idx = 0
        for batch_idx in tqdm(range(total_batches), desc=f'writing {filename}'):
            # Batch together samples for faster write
            batch = dset.shard(num_shards=total_batches, index=batch_idx, contiguous=True).with_format('numpy')
            arr_batch = np.concatenate(batch['ids'])
            # Write into mmap
            arr[idx : idx + len(arr_batch)] = arr_batch
            idx += len(arr_batch)
        arr.flush()
    # to read the bin files later, e.g. with numpy:
    # m = np.memmap('train.bin', dtype=np.uint16, mode='r')

    meta = {
        'vocab_size': enc.max_token_value + 1,  # el tamaño del vocabulario
        'encoding': 'gpt2'  # nombre del modelo de codificación
    }

    with open(os.path.join(os.path.dirname(__file__), 'meta.pkl'), 'wb') as f:
        pickle.dump(meta, f)

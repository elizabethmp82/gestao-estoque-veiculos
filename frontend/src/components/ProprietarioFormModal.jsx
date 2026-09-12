import { useState,useEffect } from 'react';
import {
  Alert,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
  Stack,
} from '@mui/material';

import api from '../services/api';

const estadoInicial = {
  nomeCompleto: '',
  cpf: '',
  dataAquisicao: '',
  dataVenda: '',
  observacao: '',
};

export default function ProprietarioFormModal({
  open,
  onClose,
  veiculoId,
  proprietario,
  onSalvo,
}) {
  const [form, setForm] = useState(estadoInicial);
  const [salvando, setSalvando] = useState(false);
  const [erro, setErro] = useState('');



  const editando = Boolean(proprietario);

useEffect(() => {
  if (proprietario) {
    setForm({
      nomeCompleto: proprietario.nomeCompleto ?? '',
      cpf: proprietario.cpf ?? '',
      dataAquisicao:
        proprietario.dataAquisicao?.substring(0, 10) ?? '',
      dataVenda:
        proprietario.dataVenda?.substring(0, 10) ?? '',
      observacao: proprietario.observacao ?? '',
    });
  } else {
    setForm(estadoInicial);
  }
}, [proprietario, open]);

  function alterarCampo(event) {
    const { name, value } = event.target;

    setForm((anterior) => ({
      ...anterior,
      [name]: value,
    }));
  }

 async function salvar() {
  setErro('');

  try {
    setSalvando(true);

    if (editando) {
      await api.put(`/Proprietarios/${proprietario.id}`, {
        nomeCompleto: form.nomeCompleto,
        cpf: form.cpf,
        dataAquisicao: form.dataAquisicao,
        dataVenda: form.dataVenda || null,
        observacao: form.observacao,
      });
    } else {
      await api.post('/Proprietarios', {
        veiculoId,
        nomeCompleto: form.nomeCompleto,
        cpf: form.cpf,
        dataAquisicao: form.dataAquisicao,
        observacao: form.observacao,
      });
    }

    setForm(estadoInicial);
    setErro('');

    await onSalvo();
    onClose();
  } catch (error) {
    console.error('Erro ao salvar proprietário:', error);

    setErro(
      error.response?.data?.mensagem ||
        'Erro ao salvar proprietário.'
    );
  } finally {
    setSalvando(false);
  }
}
  function fechar() {
  setForm(estadoInicial);
  setErro('');
  onClose();
}
  

  return (
    <Dialog
      open={open}
       onClose={fechar}
      fullWidth
      maxWidth="sm"
    >
      <DialogTitle>
       {editando
        ? 'Editar proprietário'
        : 'Cadastrar proprietário'}
      </DialogTitle>

      <DialogContent dividers>
  {erro && (
    <Alert
      severity="error"
      sx={{ mb: 2 }}
      onClose={() => setErro('')}
    >
      {erro}
    </Alert>
  )}
        <Stack spacing={2} sx={{ mt: 1 }}>
          <TextField
            label="Nome completo"
            name="nomeCompleto"
            value={form.nomeCompleto}
            onChange={alterarCampo}
            fullWidth
            required
          />

          <TextField
            label="CPF"
            name="cpf"
            value={form.cpf}
            onChange={alterarCampo}
            fullWidth
            required
          />

          <TextField
            label="Data de aquisição"
            name="dataAquisicao"
            type="date"
            value={form.dataAquisicao}
            onChange={alterarCampo}
            fullWidth
            required
            slotProps={{
              inputLabel: {
                shrink: true,
              },
            }}
          />
          {editando && (
          <TextField
            label="Data da venda"
            name="dataVenda"
            type="date"
            value={form.dataVenda}
            onChange={alterarCampo}
            fullWidth
            slotProps={{
              inputLabel: {
                shrink: true,
              },
            }}
          />
        )}

          <TextField
            label="Observação"
            name="observacao"
            value={form.observacao}
            onChange={alterarCampo}
            multiline
            rows={3}
            fullWidth
          />
        </Stack>
      </DialogContent>

      <DialogActions>
        <Button
          onClick={fechar}
          color="inherit"
        >
          Cancelar
        </Button>

        <Button
          variant="contained"
          onClick={salvar}
          disabled={salvando}
        >
          {salvando ? 'Salvando...' : 'Salvar'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
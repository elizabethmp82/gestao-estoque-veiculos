import { useEffect, useState } from 'react';
import {
  Box,
  Button,
  Chip,
  Container,
  MenuItem,
  Paper,
  Stack,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TextField,
  Typography,
  Alert,
  Snackbar,
} from '@mui/material';

import AddIcon from '@mui/icons-material/Add';
import VisibilityIcon from '@mui/icons-material/Visibility';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import VeiculoFormModal from '../components/VeiculoFormModal';
import VeiculoDetalheModal from '../components/VeiculoDetalheModal';
import ProprietarioFormModal from '../components/ProprietarioFormModal';
import ConfirmDialog from '../components/ConfirmDialog';

import api from '../services/api';

export default function VeiculosPage() {
  const [veiculos, setVeiculos] = useState([]);
  const [marca, setMarca] = useState('');
  const [situacao, setSituacao] = useState('');
  const [modalVeiculoAberto, setModalVeiculoAberto] = useState(false);
  const [modalDetalheAberto, setModalDetalheAberto] = useState(false);
  const [veiculoSelecionado, setVeiculoSelecionado] = useState(null);
  const [modalProprietarioAberto, setModalProprietarioAberto] = useState(false);
  const [proprietarioSelecionado, setProprietarioSelecionado] = useState(null);
  const [veiculoEdicao, setVeiculoEdicao] = useState(null);
  const [mensagem, setMensagem] = useState({ aberta: false,texto: '', tipo: 'success',});
  const [confirmacao, setConfirmacao] = useState({ aberta: false,  tipo: null,  item: null,});
  const [processandoConfirmacao, setProcessandoConfirmacao] = useState(false);

  useEffect(() => {
    async function buscarVeiculos() {
      try {
        const response = await api.get('/Veiculos');

        setVeiculos(response.data);
      } catch (error) {
        console.error('Erro ao carregar veículos:', error);
      }
    }

    buscarVeiculos();
  }, []);

  async function filtrarVeiculos() {
    try {
      const response = await api.get('/Veiculos', {
        params: {
          marca: marca || undefined,
          situacao: situacao || undefined,
        },
      });

      setVeiculos(response.data);
    } catch (error) {
      console.error('Erro ao filtrar veículos:', error);
    }
  }

  function formatarPreco(valor) {
    return Number(valor).toLocaleString('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    });
  }

 async function visualizarVeiculo(id) {
  try {
    const response = await api.get(`/Veiculos/${id}`);

    setVeiculoSelecionado(response.data);
    setModalDetalheAberto(true);
  } catch (error) {
    console.error('Erro ao buscar veículo:', error);
  }
    
}

  async function atualizarVeiculoSelecionado() {
  if (!veiculoSelecionado) {
    return;
  }

  try {
    const response = await api.get(
      `/Veiculos/${veiculoSelecionado.id}`,
    );

    setVeiculoSelecionado(response.data);
  } catch (error) {
    console.error(
      'Erro ao atualizar dados do veículo:',
      error,
    );
  }
}
   function editarProprietario(proprietario) {
  setProprietarioSelecionado(proprietario);
  setModalProprietarioAberto(true);
}

function solicitarExclusaoVeiculo(veiculo) {
  setConfirmacao({
    aberta: true,
    tipo: 'veiculo',
    item: veiculo,
  });
}

function solicitarExclusaoProprietario(proprietario) {
  setConfirmacao({
    aberta: true,
    tipo: 'proprietario',
    item: proprietario,
  });
}

async function confirmarExclusao() {
  if (!confirmacao.item) {
    return;
  }

  try {
    setProcessandoConfirmacao(true);

    if (confirmacao.tipo === 'veiculo') {
      await api.delete(`/Veiculos/${confirmacao.item.id}`);

      await filtrarVeiculos();

      mostrarMensagem(
        'Veículo excluído com sucesso.',
        'success'
      );
    }

    if (confirmacao.tipo === 'proprietario') {
      await api.delete(
        `/Proprietarios/${confirmacao.item.id}`
      );

      await atualizarVeiculoSelecionado();

      mostrarMensagem(
        'Proprietário excluído com sucesso.',
        'success'
      );
    }

    setConfirmacao({
      aberta: false,
      tipo: null,
      item: null,
    });
  } catch (error) {
    setConfirmacao({
      aberta: false,
      tipo: null,
      item: null,
    });

    mostrarMensagem(
      error.response?.data?.mensagem ||
        'Não foi possível concluir a exclusão.',
      'error'
    );
  } finally {
    setProcessandoConfirmacao(false);
  }
}
   
function editarVeiculo(veiculo) {
  setVeiculoEdicao(veiculo);
  setModalVeiculoAberto(true);
}



function mostrarMensagem(texto, tipo = 'success') {
  setMensagem({
    aberta: true,
    texto,
    tipo,
  });
}

  return (
    <Box
      sx={{
        minHeight: '100vh',
        bgcolor: 'background.default',
        py: {
          xs: 2,
          md: 4,
        },
      }}
    >
      <Container maxWidth="xl">
        <Stack
          direction={{
            xs: 'column',
            md: 'row',
          }}
          justifyContent="space-between"
          alignItems={{
            xs: 'stretch',
            md: 'center',
          }}
          spacing={2}
          mb={3}
        >
          <Box>
            <Typography
              variant="h4"
              fontWeight={700}
              sx={{
                fontSize: {
                  xs: '1.6rem',
                  md: '2rem',
                },
              }}
            >
              Gestão de Estoque de Veículos
            </Typography>

            <Typography
              variant="body2"
              color="text.secondary"
            >
              Gerencie veículos e proprietários.
            </Typography>
          </Box>

          <Button
             variant="contained"
             startIcon={<AddIcon />}
             size="large"
               onClick={() => {
                 setVeiculoEdicao(null);
                 setModalVeiculoAberto(true);
               }}
              sx={{
                 alignSelf: {
                 xs: 'stretch',
                 md: 'auto',
              },
            }}
           >
               Novo veículo
         </Button>
        </Stack>

        <Paper
          sx={{
            p: {
              xs: 2,
              md: 3,
            },
            mb: 3,
          }}
        >
          <Stack
            direction={{
              xs: 'column',
              md: 'row',
            }}
            spacing={2}
          >
            <TextField
              label="Marca"
              value={marca}
              onChange={(event) => setMarca(event.target.value)}
              fullWidth
            />

            <TextField
              select
              label="Situação"
              value={situacao}
              onChange={(event) => setSituacao(event.target.value)}
              fullWidth
            >
              <MenuItem value="">Todas</MenuItem>
              <MenuItem value="Disponível">Disponível</MenuItem>
              <MenuItem value="Reservado">Reservado</MenuItem>
              <MenuItem value="Vendido">Vendido</MenuItem>
            </TextField>

            <Button
              variant="outlined"
              onClick={filtrarVeiculos}
              sx={{
                minWidth: {
                  xs: '100%',
                  md: 140,
                },
              }}
            >
              Filtrar
            </Button>
          </Stack>
        </Paper>

        <Paper>
          <TableContainer
            sx={{
              overflowX: 'auto',
            }}
          >
            <Table
              sx={{
                minWidth: 900,
              }}
            >
              <TableHead>
                <TableRow>
                  <TableCell>Marca</TableCell>
                  <TableCell>Modelo</TableCell>
                  <TableCell>Ano</TableCell>
                  <TableCell>Placa</TableCell>
                  <TableCell>Tipo</TableCell>
                  <TableCell>Preço</TableCell>
                  <TableCell>Quilometragem</TableCell>
                  <TableCell>Situação</TableCell>
                  <TableCell align="center">
                    Ações
                  </TableCell>
                </TableRow>
              </TableHead>

              <TableBody>
                {veiculos.map((veiculo) => (
                  <TableRow key={veiculo.id}>
                    <TableCell>
                      {veiculo.marca}
                    </TableCell>

                    <TableCell>
                      {veiculo.modelo}
                    </TableCell>

                    <TableCell>
                      {veiculo.ano}
                    </TableCell>

                    <TableCell>
                      {veiculo.placa}
                    </TableCell>

                    <TableCell>
                      {veiculo.tipo}
                    </TableCell>

                    <TableCell>
                      {formatarPreco(veiculo.preco)}
                    </TableCell>

                    <TableCell>
                      {veiculo.quilometragem} km
                    </TableCell>

                    <TableCell>
                      <Chip
                        label={veiculo.situacao}
                        color={
                          veiculo.situacao === 'Disponível'
                            ? 'success'
                            : veiculo.situacao === 'Vendido'
                              ? 'default'
                              : 'warning'
                        }
                        size="small"
                      />
                    </TableCell>

                    <TableCell align="center">
                      <Stack
                        direction="row"
                        spacing={1}
                        justifyContent="center"
                      >
                        <Button
                          size="small"
                          startIcon={<VisibilityIcon />}
                          onClick={() => visualizarVeiculo(veiculo.id)}
                        >
                          Ver
                        </Button>

                        <Button
                          size="small"
                          startIcon={<EditIcon />}
                          onClick={() => editarVeiculo(veiculo)}
                        >
                          Editar
                        </Button>

                        <Button
                          size="small"
                          color="error"
                          startIcon={<DeleteIcon />}
                          onClick={() => solicitarExclusaoVeiculo(veiculo)}
                        >
                          Excluir
                        </Button>
                      </Stack>
                    </TableCell>
                  </TableRow>
                ))}

                {veiculos.length === 0 && (
                  <TableRow>
                    <TableCell
                      colSpan={9}
                      align="center"
                      sx={{
                        py: 5,
                        color: 'text.secondary',
                      }}
                    >
                      Nenhum veículo encontrado.
                    </TableCell>
                  </TableRow>
                )}
              </TableBody>
            </Table>
          </TableContainer>
        </Paper>
      </Container>
      <VeiculoFormModal
       open={modalVeiculoAberto}
       onClose={() => {
         setModalVeiculoAberto(false);
         setVeiculoEdicao(null);
       }}
       onSalvo={filtrarVeiculos}
       veiculo={veiculoEdicao}
     />
      <VeiculoDetalheModal
        open={modalDetalheAberto}
        onClose={() => setModalDetalheAberto(false)}
        veiculo={veiculoSelecionado}

        onAdicionarProprietario={() => {
        setProprietarioSelecionado(null);
        setModalProprietarioAberto(true);
          }}

        onEditarProprietario={editarProprietario}
        onExcluirProprietario={solicitarExclusaoProprietario}
     />
     <ProprietarioFormModal
  open={modalProprietarioAberto}

  onClose={() => {
    setModalProprietarioAberto(false);
    setProprietarioSelecionado(null);
  }}

  veiculoId={veiculoSelecionado?.id}
  proprietario={proprietarioSelecionado}
  onSalvo={atualizarVeiculoSelecionado}
/>

 <ConfirmDialog
  open={confirmacao.aberta}
  title="Confirmar exclusão"
  message={
    confirmacao.tipo === 'veiculo'
      ? `Deseja realmente excluir o veículo ${confirmacao.item?.marca} ${confirmacao.item?.modelo}?`
      : `Deseja realmente excluir o proprietário ${confirmacao.item?.nomeCompleto}?`
  }
  confirmText="Excluir"
  onConfirm={confirmarExclusao}
  onClose={() =>
    setConfirmacao({
      aberta: false,
      tipo: null,
      item: null,
    })
  }
  loading={processandoConfirmacao}
/>


  <Snackbar
  open={mensagem.aberta}
  autoHideDuration={3500}
  onClose={() =>
    setMensagem((anterior) => ({
      ...anterior,
      aberta: false,
    }))
  }
  anchorOrigin={{
    vertical: 'top',
    horizontal: 'right',
  }}
>
  <Alert
    severity={mensagem.tipo}
    variant="filled"
    onClose={() =>
      setMensagem((anterior) => ({
        ...anterior,
        aberta: false,
      }))
    }
  >
    {mensagem.texto}
  </Alert>
</Snackbar> 
    </Box>
    
  );
}
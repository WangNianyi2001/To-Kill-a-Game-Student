using System;
		cb = new CommandBuffer();
		camera.AddCommandBuffer(cbEv, cb);
	}
		cb.Clear();
		cb.Blit(buffer.rt, dest.rt);